; ModuleID = 'jni_remap.armeabi-v7a.ll'
source_filename = "jni_remap.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.JniRemappingIndexMethodEntry = type {
	%struct.JniRemappingString, ; JniRemappingString name
	%struct.JniRemappingString, ; JniRemappingString signature
	%struct.JniRemappingReplacementMethod ; JniRemappingReplacementMethod replacement
}

%struct.JniRemappingIndexTypeEntry = type {
	%struct.JniRemappingString, ; JniRemappingString name
	i32, ; uint32_t method_count
	ptr ; JniRemappingIndexMethodEntry methods
}

%struct.JniRemappingReplacementMethod = type {
	ptr, ; char* target_type
	ptr, ; char* target_name
	i8 ; bool is_static
}

%struct.JniRemappingString = type {
	i32, ; uint32_t length
	ptr ; char* str
}

%struct.JniRemappingTypeReplacementEntry = type {
	%struct.JniRemappingString, ; JniRemappingString name
	ptr ; char* replacement
}

@jni_remapping_type_replacements = dso_local local_unnamed_addr constant %struct.JniRemappingTypeReplacementEntry zeroinitializer, align 4

@jni_remapping_method_replacement_index = dso_local local_unnamed_addr constant %struct.JniRemappingIndexTypeEntry zeroinitializer, align 4

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.1xx @ af27162bee43b7fecdca59b4f67aa8c175cbc875"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
